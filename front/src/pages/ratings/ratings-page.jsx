import { useEffect, useState } from 'react'
import { Pagination, Modal } from 'antd'
import { axiosInstance as axios } from '../../axios'
import Loading from '../../components/loading'

import './ratings-page.css'
import '../../shared/styles.css'

const Ratings = () => {

    const [ratings, setRatings] = useState([])
    const [page, setPage] = useState(1)
    const [ratingsCount, setRatingsCount] = useState(0)
    const [isLoading, setIsLoading] = useState(true)

    const [modal, modalHolder] = Modal.useModal()

    const fetchRatings = () => {
        axios.get(`/ratings?page=${page}&pageSize=${10}`)
            .then(({ data }) => {
                setRatings(data.data)
                setRatingsCount(data.totalCount)
                setIsLoading(false)
            })
            .catch(_ => {
                modal.error({
                    title: 'Could not get ratings'
                })
            })
    }

    // eslint-disable-next-line react-hooks/exhaustive-deps
    useEffect(fetchRatings, [page])

    const changePage = (page, _) => {
        setPage(page)
    }

    return (
        <div className='items-container ratings-container'>
            { modalHolder }
            <div className='items-container-title'>Best players</div>
            <Loading isLoading={ isLoading }>
                <div>
                    {
                        ratings.map((rating, index) => (
                            <div key={ index } className='item'>
                                <div>{ index + 1 + 10 * (page - 1) }</div>
                                <div>{ rating.username }</div>
                                <div className='points'>{ rating.rating }</div>
                            </div>
                        ))
                    }
                </div>
                {
                    ratings.length !== 0 && 
                        <Pagination
                            responsive
                            className='pagination'
                            pageSize={ 10 }
                            total={ ratingsCount }
                            onChange={ changePage }/>
                }
            </Loading>
        </div>
    )
}

export default Ratings