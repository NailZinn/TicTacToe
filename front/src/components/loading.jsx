import { Spin } from 'antd'

import './loading.css'

const Loading = ({ isLoading, children }) => {

    return (
        isLoading
        ?
        <Spin className='spin' size='large' />
        :
        <>
            { children }
        </>
    )
}

export default Loading