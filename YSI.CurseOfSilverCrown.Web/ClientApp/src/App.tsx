import * as React from 'react';
import { Route, Routes } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';

import './custom.css'
import Redirect from './components/Redirect';
import Constants from './Constants';


export default () => {

    return (
        <Layout>
            <Routes>
                <Route path='/' element={<Home />} />
                <Route path='/counter' element={<Counter />} />
                <Route path='/fetch-data' element={<FetchData />}>
                    <Route path=':startDateIndex' element={<FetchData />} />
                </Route>
                
                <Route path='/Domain' element={<Redirect route={`${Constants.mainPath}Domain`} />} />
                <Route path='/Map' element={<Redirect route={`${Constants.mainPath}Map`} />} />
                <Route path='/History' element={<Redirect route={`${Constants.mainPath}History`} />} />
                <Route path='/Organizations' element={<Redirect route={`${Constants.mainPath}Organizations`} />} />
                <Route path='/Leave' element={<Redirect route={`${Constants.mainPath}Leave`} />} />
                <Route path='/Register' element={<Redirect route={`${Constants.mainPath}Identity/Account/Register`} />} />
                <Route path='/Login' element={<Redirect route={`${Constants.mainPath}Identity/Account/Login`} />} />
            </Routes>
        </Layout>
    )
};
