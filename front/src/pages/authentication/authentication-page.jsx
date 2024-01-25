import { Form, Input, Button } from 'antd'

import './authentication-page.css'

const Authentication = () => {

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
                <Form.Item 
                    name='repeatedPassword'
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
        </div>
    )
}

export default Authentication