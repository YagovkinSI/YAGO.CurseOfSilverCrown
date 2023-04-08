import * as React from 'react';
import { Route, Routes } from 'react-router';
import Layout from './components/Layout';
import Main from './components/Main';

import './index.css'

export default () => (
    <Layout>
        <Routes>
            <Route path='/' element={<Main />} />
        </Routes>
    </Layout>
);
