import * as React from 'react';
import { Route, Routes } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';

import './custom.css'
import LoginRegister from './components/LoginRegister';
import Logout from './components/Logout';
import { useAppSelector } from './store';

export default () => {
    const state = useAppSelector(state => state.userReducer);
    console.log(state);

    return (
        state.isSignedIn
        ?
        <Layout>
            <Routes>
                <Route path='/' element={<Home />} />
                <Route path='/counter' element={<Counter />} />
                <Route path='/fetch-data' element={<FetchData />}>
                    <Route path=':startDateIndex' element={<FetchData />} />
                </Route>
                <Route path='/Logout' element={<Logout />} />
               
                <Route path='*' element={<Home />} />
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
               
                <Route path='*' element={<Home />} />
             </Routes>
        </Layout>
    )
};
