import { Form, Input, Button } from 'antd'
import { axiosInstance as axios } from '../../axios'
import { useNavigate } from 'react-router-dom'
import { Modal } from 'antd'

import './authorization-page.css'

const Authorization = () => {

    const navigate = useNavigate()
    const [modal, modalHolder] = Modal.useModal()

    const getRequiredRule = (message) => {
        return {
            required: true,
            message: message
        }
    }

    const sendForm = (values) => {
        axios.post(`/auth/login`, values)
            .then(_ => navigate('/'))
            .catch(({ response }) => {
                modal.error({
                    title: `Could not authorize user: ${response?.data?.errors?.map(e => e.description)?.join(', ') ?? 'something went wrong'}`
                })
            })
    }

    return (
        <div className='container'>
            { modalHolder }
            <Form onFinish={ sendForm } layout='vertical'>
                <Form.Item 
                    name='username'
                    label='Your name'
                    rules={[ getRequiredRule('Name is required!') ]}>
                    <Input />
                </Form.Item>
                <Form.Item 
                    name='password'
                    label='Your password'
                    rules={[ getRequiredRule('Password is required!') ]}>
                    <Input.Password />
                </Form.Item>
                <Form.Item>
                    <Button htmlType='submit'>
                        Sign in
                    </Button>
                </Form.Item>
            </Form>
            <div className='sign-button' onClick={ () => navigate('/authentication') }>
                Sign up
            </div>
        </div>
    )
}

export default Authorization