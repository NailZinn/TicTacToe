import { useState, useEffect } from 'react'
import { Modal, Button } from 'antd'
import { HttpTransportType, HubConnectionBuilder } from '@microsoft/signalr'

import './game-page.css'

const Game = () => {

    const [squares, setSquares] = useState(Array(9).fill(''))
    const [isFinished, setIsFinished] = useState(false);
    const [connection, setConnection] = useState(null)

    useEffect(() => {
        if (!connection) {
            const newConnection = new HubConnectionBuilder()
                .withUrl('https://localhost:7051/gameHub', {
                    skipNegotiation: true,
                    transport: HttpTransportType.WebSockets
                })
                .build()
            setConnection(newConnection)
        }
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    useEffect(() => {
        if (connection) {
            connection.start()
        }
    }, [connection])

    const updateSquare = (square, value) => {
        squares[square] = value;
        setSquares([ ...squares ])
        const maybeWinner = calculateWinner()
        maybeWinner && setIsFinished(true)
    }

    const calculateWinner = () => {
        const lines = [[0, 1, 2], [3, 4, 5], [6, 7, 8], [0, 3, 6], [1, 4, 7], [2, 5, 8], [0, 4, 8], [2, 4, 6]]

        for (let i = 0; i < lines.length; i++) {
            const [a, b, c] = lines[i]
            if (squares[a] && squares[a] === squares[b] && squares[a] === squares[c]) {
                return squares[a]
            }
        }

        return null;
    }

    const finishGame = () => {
        setIsFinished(false)
        setSquares(Array(9).fill(''))
    }

    return (
        <div className='game-field'>
            <div className='game-field-row'>
                <GameCell 
                    value={ squares[0] } 
                    color={ squares[0] === 'X' ? 'cadetBlue' : 'crimson' } 
                    onClick={ () => updateSquare(0, 'X') } />
                <GameCell 
                    value={ squares[1] } 
                    color={ squares[1] === 'X' ? 'cadetBlue' : 'crimson' }
                    onClick={ () => updateSquare(1, 'X') } />
                <GameCell 
                    value={ squares[2] } 
                    color={ squares[2] === 'X' ? 'cadetBlue' : 'crimson' }
                    onClick={ () => updateSquare(2, 'X') } />

            </div>
            <div className='game-field-row'>
                <GameCell 
                    value={ squares[3] } 
                    color={ squares[3] === 'X' ? 'cadetBlue' : 'crimson' }
                    onClick={ () => updateSquare(3, 'O') } />
                <GameCell 
                    value={ squares[4] } 
                    color={ squares[4] === 'X' ? 'cadetBlue' : 'crimson' }
                    onClick={ () => updateSquare(4, 'O') } />
                <GameCell 
                    value={ squares[5] } 
                    color={ squares[5] === 'X' ? 'cadetBlue' : 'crimson' }
                    onClick={ () => updateSquare(5, 'X') } />
            </div>
            <div className='game-field-row'>
                <GameCell 
                    value={ squares[6] } 
                    color={ squares[6] === 'X' ? 'cadetBlue' : 'crimson' }
                    onClick={ () => updateSquare(6, 'X') } />
                <GameCell 
                    value={ squares[7] } 
                    color={ squares[7] === 'X' ? 'cadetBlue' : 'crimson' }
                    onClick={ () => updateSquare(7, 'O') } />
                <GameCell 
                    value={ squares[8] } 
                    color={ squares[8] === 'X' ? 'cadetBlue' : 'crimson' }
                    onClick={ () => updateSquare(8, 'X') } />
            </div>
            <Modal
                open={ isFinished }
                closeIcon={ null }
                footer={ null }>
                <div>
                    <div>You win</div>
                    <Button type='primary' onClick={ finishGame }>Close</Button>
                </div>
            </Modal>
        </div>
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