import Layout from './Layout'
import HomePage from './pages/HomePage'
import { Route, Routes } from 'react-router-dom';
import ExternalRedirect from './pages/ExternalRedirect';

function App() {
  return (
    <Layout>
      <Routes>
        <Route path='/app' element={<HomePage />} />

        <Route path="/" element={<ExternalRedirect to="/" />} />
        <Route path="/Identity/Account/Register" element={<ExternalRedirect to="/Identity/Account/Register" />} />
        <Route path="/Identity/Account/Login" element={<ExternalRedirect to="/Identity/Account/Login" />} />
        <Route path="/Identity/Account/Logout" element={<ExternalRedirect to="/Identity/Account/Logout" />} />
      </Routes>
    </Layout>
  )
}

export default App;