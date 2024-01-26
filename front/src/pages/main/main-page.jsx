import { useEffect, useState} from 'react'
import { useNavigate } from 'react-router-dom'
import { Modal, Form, Input, Button } from 'antd'
import { axiosInstance } from "../../axios";

import './main-page.css'
import '../../shared/styles.css'

const MainPage = () => {

    const navigate = useNavigate()

    const pageSize = 15;
    const [isModalOpened, setIsModalOpened] = useState(false)
    const [numberOfFetches, setNumberOfFetches] = useState(0)
    const [games, setGames] = useState([])
    const [showLabel, setShowLabel] = useState(false)
    const [labelText, setLabelText] = useState("")

    useEffect(() => {
        fetchGamesAxios()
    }, [])
    
    const fetchGamesAxios = () => {
        axiosInstance.get(`/games?page=${numberOfFetches + 1}&pageSize=${pageSize}`)
            .then(response => response.data)
            .then(data => {
                setGames(prev => [...prev, ...data.data.map(mapGame)])
                setNumberOfFetches(x => x + 1)
            })
            .catch(e => {
                navigate('/authorization')
            })
    }
    
    const mapGame = (game) => {
        const formatDate = (date) => {
            date = new Date(date);
            const day = String(date.getDate()).padStart(2, '0');
            const month = String(date.getMonth() + 1).padStart(2, '0');
            const year = date.getFullYear();
            const hours = String(date.getHours()).padStart(2, '0');
            const minutes = String(date.getMinutes()).padStart(2, '0');

            return `${day}-${month}-${year} ${hours}:${minutes}`;
        }
        
        return {
            id: game.id,
            username: game.ownerUserName,
            createdAt: formatDate(game.createdAt),
            enterMessage: game.status === 0 ? 'Play' : 'Watch'
        }
    }
    const fetchGames = (e) => {
        if (e.currentTarget.scrollTop >= 185 + numberOfFetches * 45 * pageSize) {
            fetchGamesAxios()
        }
    }

    const sendForm = (values) => {
        axiosInstance.post(`/games`, values)
            .then(response => response.data)
            .then(data => {
                if (data)
                    navigate(`/game/${data.id}`)
                else
                    showLabelFunc('Ваш рейтинг выше выбранного максимального')
            })
            .catch(e => {
                navigate('/')
            })
        setIsModalOpened(false)
    }
    
    const joinGame = (gameId) => {
        axiosInstance.post(`/games/join`, {gameId})
            .then(response => response.data)
            .then(data => {
                if (data.success)
                    navigate(`/game/${gameId}`)
                else 
                {
                    showLabelFunc('У вас слишком высокий рейтинг')
                    navigate(`/`)
                }
            })
            .catch(e => {
                showLabelFunc('Не удалось подключиться к игре')
            })
    }
    
    const showLabelFunc = (text) => {
        setLabelText(text)
        setShowLabel(true)
        setTimeout(() => setShowLabel(false), 2000)
    }

    return (
        <div className='main-content' onScroll={ fetchGames }>
            <div className='buttons'>
                <div className='button' onClick={ _ => navigate('/ratings') }>Ratings</div>
                <div className='button' onClick={ _ => setIsModalOpened(true) }>Create game</div>
            </div>
            <div className='items-container'>
                <div className='items-container-title'>Available games</div>
                <div>
                    {
                        games.map(game => (
                            <div key={ game.id } className='item'>
                                <div>{ game.id }</div>
                                <div>{ game.username }</div>
                                <div>{ game.createdAt }</div>
                                <div className='enter-message' onClick={ _ => joinGame(game.id) }>{ game.enterMessage }</div>
                            </div>
                        ))
                    }
                </div>
            </div>
            {
                showLabel &&
                <label className='error-label'>
                    {labelText}
                </label>
            }
            <Modal
                open={ isModalOpened }
                onOk={ () => setIsModalOpened(false) }
                onCancel={ () => setIsModalOpened(false) }
                closeIcon={ null }
                footer={ null }>
                <Form onFinish={ sendForm }>
                    <Form.Item
                        name='maxRating'
                        label='max rating'
                        rules={[
                            {
                                required: true,
                                message: 'Max rating is requried'
                            }
                        ]}>
                        <Input type='number' />
                    </Form.Item>
                    <Form.Item>
                        <Button type='primary' htmlType='submit'>Create</Button>
                    </Form.Item>
                </Form>
            </Modal>
        </div>
    )
}

export default MainPage