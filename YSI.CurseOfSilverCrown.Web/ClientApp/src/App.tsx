import * as React from 'react';
import { Route, Routes } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';

import './custom.css'
import Register from './components/Register';

export default () => (
    <Layout>
        <Routes>
            <Route path='/' element={<Home />} />
            <Route path='/counter' element={<Counter />} />
            <Route path='/fetch-data' element={<FetchData />}>
                <Route path=':startDateIndex' element={<FetchData />} />
            </Route>
            <Route path='/Register' element={<Register />} />
        </Routes>
    </Layout>
);
