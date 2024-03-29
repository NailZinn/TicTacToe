import { useState, useEffect, useRef } from 'react'
import { Modal, Form, Input, Button } from 'antd'
import { useLocation, useNavigate } from 'react-router-dom'
import { HttpTransportType, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr'
import { axiosInstance as axios } from '../../axios'

import './game-page.css'
import {axiosInstance} from "../../axios";

const Game = () => {

    const navigate = useNavigate()
    
    const [board, setBoard] = useState(Array(9).fill(''))
    const [connection, setConnection] = useState(null)
    const [playerSymbol, setPlayerSymbol] = useState(null)
    const [playerTurn, setPlayerTurn] = useState(false)
    const [isWatcher, setIsWatcher] = useState(false)
    const [messages, setMessages] = useState([])

    const [form] = Form.useForm()
    const [modal, modalHolder] = Modal.useModal()
    const messagesEnd = useRef(null)

    const gameId = useLocation().pathname.split('/')[2] 

    useEffect(() => {
        if (!connection) {
            axiosInstance.get(`/games/${gameId}`)
                .then(resp => resp.data)
                .then(data => {
                    if (!data)
                        navigate('/')
                })
                .then(_ => {
                    const newConnection = new HubConnectionBuilder()
                        .withUrl('http://localhost:7051/gameHub', {
                            skipNegotiation: true,
                            transport: HttpTransportType.WebSockets
                        })
                        .build()
                    setConnection(newConnection)
                })
                .catch(e => {
                    navigate('/authorization')
                })
        }
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    useEffect(() => {
        if (!connection) return

        if (connection.state === HubConnectionState.Disconnected) {
            connection.start()
                .then(() => {
                    connection.invoke('JoinGameAsync', gameId)
                })
                .catch((e) => {
                    console.log(e)
                })
        }
        connection.off('ReceiveStartMessageAsync')
        connection.off('ReceiveWatcherMessageAsync')
        connection.off('ReceiveGameEventMessage')
        connection.off('ReceiveOpponentLeftGameMessage')
        connection.off('ReceiveGameChatMessageAsync')
        connection.on('ReceiveStartMessageAsync', (message) => {
            setPlayerSymbol(message.playerSymbol)
            setPlayerTurn(message.playerTurn)
            setMessages(message.gameMessages)
        })
        connection.on('ReceiveWatcherMessageAsync', (message, gameChat) => {
            setIsWatcher(true)
            setBoard(message.map(x => x === ' ' ? '' : x))
            setMessages(gameChat)
        })
        connection.on('ReceiveGameEventMessage', (message) => {
            board[message.square] = message.symbol
            setBoard([ ...board ])
            const maybeWinner = calculateWinner();
            (maybeWinner || board.every(x => x !== '')) && handleGameEnding(maybeWinner)
            setPlayerTurn(prev => !prev)
        })
        connection.on('ReceiveOpponentLeftGameMessage', () => {
            modal.success({
                title: 'Game finished.',
                content: 'Player left the game',
                okText: 'Leave game',
                onOk: () => {
                    connection.stop()
                    navigate('/')
                }
            })
        })
        connection.on('ReceiveGameChatMessageAsync', (message) => {
            setMessages(prev => [ ...prev, message ])
        })
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [connection, playerTurn, board])

    useEffect(() => {
        if (messagesEnd) {
            messagesEnd.current?.scrollIntoView({ behavior: 'smooth' })
        }
    }, [messages])

    const updateSquare = (square, value) => {
        if (!playerTurn || isWatcher || !value || board[square]) {
            return
        }
        connection.invoke('PlaceSymbolAsync', {
            gameId: gameId,
            square: square,
            symbol: value
        })
    }

    const calculateWinner = () => {
        const lines = [[0, 1, 2], [3, 4, 5], [6, 7, 8], [0, 3, 6], [1, 4, 7], [2, 5, 8], [0, 4, 8], [2, 4, 6]]

        for (let i = 0; i < lines.length; i++) {
            const [a, b, c] = lines[i]
            if (board[a] && board[a] === board[b] && board[a] === board[c]) {
                return board[a]
            }
        }

        return null;
    }

    const handleGameEnding = (winner) => {
        let modalMessage = ''
        let reason = 0
        if (!isWatcher) {
            if (winner) {
                modalMessage = winner === playerSymbol ? 'You win. Rating +3' : 'You lose. Rating -1'
                reason = winner === playerSymbol ? 3 : -1
            } else {
                modalMessage = 'Draw'
            }
        }
        const instance = modal.success({
            title: 'Game finished',
            content: modalMessage,
            okText: 'Leave game',
            onOk: () => { 
                connection.invoke('LeaveGameAsync')
                    .then(_ => {
                        connection.stop()
                        navigate('/')
                    })
                    .catch(_ => {
                        connection.stop()
                        navigate('/')    
                    })
            }
        })
        setTimeout(() => {
            instance.destroy()
            setBoard(Array(9).fill(''))
            if (!isWatcher) {
                if (connection.state === 'Connected')
                    connection.invoke('ResetBoard', gameId)
                setPlayerSymbol(playerSymbol === 'X' ? 'O' : 'X')
            }
        }, 5000)
        axios.put(`/rating?reason=${reason}`)
            .catch(_ => console.log('something went wrong'))
    }

    const sendMessage = (values) => {
        form.setFieldValue('message', undefined)
        connection?.state === HubConnectionState.Connected && connection.invoke('SendMessageAsync', {
            gameId: gameId,
            message: values.message
        })
    }

    return (
        <>
            <div className='game-chat'>
                <div className='game-chat-container'>
                    <div className='message-list'>                        
                        {
                            messages.map((x, i) => (
                                <div 
                                    key={ i } 
                                    className='chat-message'
                                    style={{ color: playerSymbol === 'X' ? 'cadetblue' : 'crimson' }}>
                                    { x }
                                </div>
                            ))
                        }
                        <div ref={ messagesEnd }></div>
                    </div>                  
                    <Form form={ form } onFinish={ sendMessage }>
                        <Form.Item name='message' rules={[{
                            required: true,
                            message: 'entere message'
                        }]}>
                            <Input />
                        </Form.Item>
                        <Form.Item hidden>
                            <Button htmlType='submit'></Button>
                        </Form.Item>
                    </Form>
                </div>
            </div>
            <div className='game-field'>
                <div className='game-field-row'>
                    <GameCell 
                        value={ board[0] } 
                        color={ board[0] === 'X' ? 'cadetBlue' : 'crimson' } 
                        onClick={ () => updateSquare(0, playerSymbol) } />
                    <GameCell 
                        value={ board[1] } 
                        color={ board[1] === 'X' ? 'cadetBlue' : 'crimson' }
                        onClick={ () => updateSquare(1, playerSymbol) } />
                    <GameCell 
                        value={ board[2] } 
                        color={ board[2] === 'X' ? 'cadetBlue' : 'crimson' }
                        onClick={ () => updateSquare(2, playerSymbol) } />
                </div>
                <div className='game-field-row'>
                    <GameCell 
                        value={ board[3] } 
                        color={ board[3] === 'X' ? 'cadetBlue' : 'crimson' }
                        onClick={ () => updateSquare(3, playerSymbol) } />
                    <GameCell 
                        value={ board[4] } 
                        color={ board[4] === 'X' ? 'cadetBlue' : 'crimson' }
                        onClick={ () => updateSquare(4, playerSymbol) } />
                    <GameCell 
                        value={ board[5] } 
                        color={ board[5] === 'X' ? 'cadetBlue' : 'crimson' }
                        onClick={ () => updateSquare(5, playerSymbol) } />
                </div>
                <div className='game-field-row'>
                    <GameCell 
                        value={ board[6] } 
                        color={ board[6] === 'X' ? 'cadetBlue' : 'crimson' }
                        onClick={ () => updateSquare(6, playerSymbol) } />
                    <GameCell 
                        value={ board[7] } 
                        color={ board[7] === 'X' ? 'cadetBlue' : 'crimson' }
                        onClick={ () => updateSquare(7, playerSymbol) } />
                    <GameCell 
                        value={ board[8] } 
                        color={ board[8] === 'X' ? 'cadetBlue' : 'crimson' }
                        onClick={ () => updateSquare(8, playerSymbol) } />
                </div>
                { modalHolder }
            </div>
            {
                playerSymbol &&
                    <div 
                        style={{ color: playerTurn 
                            ? playerSymbol === 'X' ? 'cadetblue' : 'crimson'
                            : playerSymbol !== 'X' ? 'cadetblue' : 'crimson' }}
                        className='game-turn-message'>
                        { playerTurn ? 'Your turn' : 'Opponent\'s turn' }
                    </div>
            }
        </>
    )
}

const GameCell = ({ value, color, onClick }) => {
    
    return (
        <div style={{ color: color }} className='game-cell' onClick={ onClick }>
            { value }
        </div>
    )
}

export default Game