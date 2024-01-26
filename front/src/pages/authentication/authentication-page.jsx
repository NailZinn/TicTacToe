import { Form, Input, Button } from 'antd'
import { axiosInstance as axios } from '../../axios'
import { useNavigate } from 'react-router-dom'
import { Modal } from 'antd'

import './authentication-page.css'
import '../../shared/styles.css'

const Authentication = () => {

    const navigate = useNavigate()
    const [modal, modalHolder] = Modal.useModal()

    const getRequiredRule = (message) => {
        return {
            required: true,
            message: message
        }
    }

    const sendForm = (values) => {
        axios.post(`/auth/register`, values)
            .then(_ => navigate('/'))
            .catch(({ response }) => {
                modal.error({
                    title: `Could not authenticate user: ${response?.data?.errors?.map(e => e.description)?.join(', ') ?? 'something went wrong'}`
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
                <Form.Item 
                    name='repeatPassword'
                    label='Repeat your password'
                    rules={[ 
                        getRequiredRule('Repeat your password!'),
                        ({ getFieldValue }) => ({
                            validator(_, value) {
                              if (!value || getFieldValue('password') === value) {
                                return Promise.resolve();
                              }
                              return Promise.reject(new Error('Passwords must be equal'));
                            },
                        }) ]}>
                    <Input.Password />
                </Form.Item>
                <Form.Item>
                    <Button htmlType='submit'>
                        Sign up
                    </Button>
                </Form.Item>
            </Form>
            <div className='sign-button' onClick={ () => navigate('/authorization') }>
                Sign in
            </div>
        </div>
    )
}

export default Authentication