import * as React from 'react';
import { Route, Routes } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';

import './custom.css'
import LoginRegister from './components/LoginRegister';

export default () => (
    <Layout>
        <Routes>
            <Route path='/' element={<Home />} />
            <Route path='/counter' element={<Counter />} />
            <Route path='/fetch-data' element={<FetchData />}>
                <Route path=':startDateIndex' element={<FetchData />} />
            </Route>
            <Route path='/Register' element={<LoginRegister isLogin={false} />} />
            <Route path='/Login' element={<LoginRegister isLogin={true} />} />
        </Routes>
    </Layout>
);
