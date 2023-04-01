import * as React from 'react';
import { Route, Routes } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';

export default () => (
    <Layout>
        <Routes>
            <Route path='/' element={<Home />} />
        </Routes>
    </Layout>
);
