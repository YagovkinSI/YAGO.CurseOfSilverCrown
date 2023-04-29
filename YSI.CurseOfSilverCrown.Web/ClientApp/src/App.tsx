import * as React from 'react';
import { Route, Routes } from 'react-router';
import { useAppDispatch, useAppSelector } from './store';
import { userActionCreators } from './store/User';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';
import LoginRegister from './components/LoginRegister';
import Logout from './components/Logout';
import Developing from './components/Developing';
import Map from './components/Map';

import './custom.css'

export default () => {
    const state = useAppSelector(state => state.userReducer);
    const dispatch = useAppDispatch();

    userActionCreators.getCurrentUser(dispatch);

    return (
        state.user != undefined
        ?
        <Layout>
            <Routes>
                <Route path='/' element={<Home />} />
                <Route path='/counter' element={<Counter />} />
                <Route path='/fetch-data' element={<FetchData />}>
                    <Route path=':startDateIndex' element={<FetchData />} />
                </Route>
                <Route path='/Logout' element={<Logout />} />
                <Route path='/Map' element={<Map />} />
               
                <Route path='*' element={<Developing />} />
             </Routes>
        </Layout>
        :
        <Layout>
            <Routes>
                <Route path='/' element={<Home />} />
                <Route path='/counter' element={<Counter />} />
                <Route path='/fetch-data' element={<FetchData />}>
                    <Route path=':startDateIndex' element={<FetchData />} />
                </Route>
                <Route path='/Register' element={<LoginRegister isLogin={false} />} />
                <Route path='/Login' element={<LoginRegister isLogin={true}/>} />
                <Route path='/Map' element={<Map />} />
               
                <Route path='*' element={<Developing />} />
             </Routes>
        </Layout>
    )
};
