import { Form, Input, Button } from 'antd'

import './authorization-page.css'

const Authorization = () => {

    const getRequiredRule = (message) => {
        return {
            required: true,
            message: message
        }
    }

    const sendForm = (values) => {

    }

    return (
        <div className='container'>
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
        </div>
    )
}

export default Authorization