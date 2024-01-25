import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { Modal, Form, Input, Button } from 'antd'

import './main-page.css'
import '../../shared/styles.css'

const MainPage = () => {

    const navigate = useNavigate()

    const [isModalOpened, setIsModalOpened] = useState(false)
    const [numberOfFetches, setNumberOfFetches] = useState(0)
    const [games, setGames] = useState([
        {
            username: 'User 1',
            createdAt: '25-01-2024',
            id: '123',
            enterMessage: 'Play'
        },
        {
            username: 'User 2',
            createdAt: '25-01-2024',
            id: '124',
            enterMessage: 'Play'
        },
        {
            username: 'User 3',
            createdAt: '25-01-2024',
            id: '125',
            enterMessage: 'Play'
        },
        {
            username: 'User 4',
            createdAt: '25-01-2024',
            id: '126',
            enterMessage: 'Play'
        },
        {
            username: 'User 5',
            createdAt: '25-01-2024',
            id: '127',
            enterMessage: 'Watch'
        },
        {
            username: 'User 6',
            createdAt: '25-01-2024',
            id: '128',
            enterMessage: 'Watch'
        },
        {
            username: 'User 7',
            createdAt: '25-01-2024',
            id: '129',
            enterMessage: 'Watch'
        },
        {
            username: 'User 8',
            createdAt: '25-01-2024',
            id: '130',
            enterMessage: 'Watch'
        },
        {
            username: 'User 9',
            createdAt: '25-01-2024',
            id: '131',
            enterMessage: 'Watch'
        },
        {
            username: 'User 10',
            createdAt: '25-01-2024',
            id: '132',
            enterMessage: 'Watch'
        },
        {
            username: 'User 11',
            createdAt: '25-01-2024',
            id: '133',
            enterMessage: 'Watch'
        },
        {
            username: 'User 12',
            createdAt: '25-01-2024',
            id: '134',
            enterMessage: 'Watch'
        },
        {
            username: 'User 13',
            createdAt: '25-01-2024',
            id: '135',
            enterMessage: 'Watch'
        },
        {
            username: 'User 14',
            createdAt: '25-01-2024',
            id: '136',
            enterMessage: 'Watch'
        },
        {
            username: 'User 15',
            createdAt: '25-01-2024',
            id: '137',
            enterMessage: 'Watch'
        },
    ])

    const fetchGames = (e) => {
        if (e.currentTarget.scrollTop >= 185 + numberOfFetches * 45) {
            setGames(prev => [ ...prev, {
                username: 'User 16',
                createdAt: '25-01-2024',
                id: 138 + numberOfFetches,
                enterMessage: 'Watch'
            }])
            setNumberOfFetches(numberOfFetches + 1)
        }
    }

    const sendForm = (values) => {
        setIsModalOpened(false)
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
                                <div className='enter-message'>{ game.enterMessage }</div>
                            </div>
                        ))
                    }
                </div>
            </div>
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