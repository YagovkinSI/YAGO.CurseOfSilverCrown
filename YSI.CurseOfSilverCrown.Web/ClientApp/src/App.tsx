import * as React from 'react';
import { Route, Routes } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';

import './custom.css'
import Redirect from './components/Redirect';
import Constants from './Constants';
import LoginRegister from './components/LoginRegister';
import Logout from './components/Logout';

export default () => {
    return (
        <Layout>
            <Routes>
                <Route path='/' element={<Home />} />
                <Route path='/counter' element={<Counter />} />
                <Route path='/fetch-data' element={<FetchData />}>
                    <Route path=':startDateIndex' element={<FetchData />} />
                </Route>
                <Route path='/Register' element={<LoginRegister isLogin={false} />} />
                <Route path='/Login' element={<LoginRegister isLogin={true}/>} />
                <Route path='/Logout' element={<Logout />} />
               
                <Route path='/Domain' element={<Redirect route={`${Constants.mainPath}Domain`} />} />
                <Route path='/Map' element={<Redirect route={`${Constants.mainPath}Map`} />} />
                <Route path='/History' element={<Redirect route={`${Constants.mainPath}History`} />} />
                <Route path='/Organizations' element={<Redirect route={`${Constants.mainPath}Organizations`} />} />
             </Routes>
        </Layout>
    )
};
