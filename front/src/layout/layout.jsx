import { useNavigate } from 'react-router-dom'
import { axiosInstance as axios } from '../axios'

import './layout.css'

const Layout = ({ children }) => {
    
    const navigate = useNavigate();

    const logout = () => {
        axios.post('/auth/logout')
            .then(_ => navigate('/authorization'))
    }

    return (
        <>
            <div className='header'>
                <div className='game-title' onClick={ _ => navigate('/') }>Tic Tac Toe</div>
                <div className='logout' onClick={ _ => logout() }>logout</div>
            </div>
            <div className='body'>
                { children }
            </div>
        </>
    )
}

export default Layout;