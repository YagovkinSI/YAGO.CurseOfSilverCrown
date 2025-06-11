import Layout from './Layout'
import HomePage from './HomePage'
import { Route, Routes } from 'react-router-dom';
import ProvincePage from './ProvincePage';
import ExternalRedirect from './pages/ExternalRedirect';

function App() {

  return (
    <Layout>
      <Routes>
        <Route path='/app' element={<HomePage />} />
        <Route path='/app/map' />
        <Route path='/app/province/:id?' element={<ProvincePage />} />

        <Route path="/" element={<ExternalRedirect to="/" />} />
        <Route path="/Domain" element={<ExternalRedirect to="/Domain" />} />
        <Route path="/History" element={<ExternalRedirect to="/History" />} />
        <Route path="/Organizations" element={<ExternalRedirect to="/Organizations" />} />
        <Route path="/Identity/Account/Register" element={<ExternalRedirect to="/Identity/Account/Register" />} />
        <Route path="/Identity/Account/Login" element={<ExternalRedirect to="/Identity/Account/Login" />} />
        <Route path="/Identity/Account/Logout" element={<ExternalRedirect to="/Identity/Account/Logout" />} />
      </Routes>
    </Layout>
  )
}

export default App;