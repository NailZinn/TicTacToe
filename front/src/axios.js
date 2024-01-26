import axios from 'axios'

export const axiosInstance = axios.create({
    baseURL: 'http://localhost:7051',
    withCredentials: true
})