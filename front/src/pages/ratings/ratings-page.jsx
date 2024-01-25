import { useState } from 'react'
import { Pagination } from 'antd'

import './ratings-page.css'
import '../../shared/styles.css'

const Ratings = () => {

    const [ratings, setRatings] = useState(Array(10).fill()
        .map(_ => ({
            username: 'User ',
            points: 100,
            position: 1
        })))

    const fetchRatings = () => {
        setRatings(Array(10).fill()
            .map(_ => ({
                username: 'User ',
                points: 90,
                position: 11
            })))
    }

    return (
        <div className='items-container ratings-container'>
            <div className='items-container-title'>Best players</div>
            <div>
                {
                    ratings.map((rating, index) => (
                        <div key={ index } className='item'>
                            <div>{ rating.position + index }</div>
                            <div>{ rating.username + (rating.position + index) }</div>
                            <div className='points'>{ rating.points - index }</div>
                        </div>
                    ))
                }
            </div>
            <Pagination
                responsive
                className='pagination'
                pageSize={ 10 }
                total={ 20 }
                onChange={ fetchRatings }/>
        </div>
    )
}

export default Ratings