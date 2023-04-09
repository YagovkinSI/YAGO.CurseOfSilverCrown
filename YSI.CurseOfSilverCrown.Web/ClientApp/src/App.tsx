import * as React from 'react';
import { Route, Routes } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';

import './custom.css'
import Redirect from './components/Redirect';


export default () => {
    const path = 'https://almusahan.ru/';

    return (
        <Layout>
            <Routes>
                <Route path='/' element={<Home />} />
                <Route path='/counter' element={<Counter />} />
                <Route path='/fetch-data' element={<FetchData />}>
                    <Route path=':startDateIndex' element={<FetchData />} />
                </Route>
                
                <Route path='/Domain' element={<Redirect route={`${path}Domain`} />} />
                <Route path='/Map' element={<Redirect route={`${path}Map`} />} />
                <Route path='/History' element={<Redirect route={`${path}History`} />} />
                <Route path='/Organizations' element={<Redirect route={`${path}Organizations`} />} />
                <Route path='/Leave' element={<Redirect route={`${path}Leave`} />} />
                <Route path='/Register' element={<Redirect route={`${path}Identity/Account/Register`} />} />
                <Route path='/Login' element={<Redirect route={`${path}Identity/Account/Login`} />} />
            </Routes>
        </Layout>
    )
};
