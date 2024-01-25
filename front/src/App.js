import { Route, Routes } from 'react-router-dom'
import './App.css';
import Layout from './layout/layout';
import Authentication from './pages/authentication/authentication-page';
import Authorization from './pages/authorization/authorization-page';
import MainPage from './pages/main/main-page';
import Ratings from './pages/ratings/ratings-page';
import Game from './pages/game/game-page';

const App = () => {

    return (
        <Layout>
            <Routes>
                <Route path='/' element={ <MainPage /> } />
                <Route path='/authentication' element={ <Authentication /> } />
                <Route path='/authorization' element={ <Authorization /> } />
                <Route path='/ratings' element={ <Ratings /> } />
                <Route path='/game/:id' element={ <Game /> }/>
            </Routes>
        </Layout>
    )
}

export default App;