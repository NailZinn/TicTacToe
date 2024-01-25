import { useNavigate } from 'react-router-dom'

import './layout.css'

const Layout = ({ children }) => {
    
    const navigate = useNavigate();

    return (
        <>
            <div className='header'>
                <div className='game-title' onClick={ _ => navigate('/') }>Tic Tac Toe</div>
            </div>
            <div className='body'>
                { children }
            </div>
        </>
    )
}

export default Layout;