import Layout from './Layout'
import HomePage from './pages/HomePage'
import { Route, Routes, useParams } from 'react-router-dom';
import ProvincePage from './pages/ProvincePage';
import ExternalRedirect from './pages/ExternalRedirect';
import FactionListPage from './pages/FactionListPage';
import RegistrationPage from './pages/RegistrationPage';
import LogoutPage from './pages/LogoutPage';

function App() {

  const OrganizationsTakeRedirect = () => {
    const { id } = useParams<{ id: string }>();
    return <ExternalRedirect to={`/Organizations/Take/${id}`} />;
  };

  return (
    <Layout>
      <Routes>
        <Route path='/app' element={<HomePage />} />
        <Route path='/app/map' />
        <Route path='/app/province/details/:id?' element={<ProvincePage />} />
        <Route path='/app/factions' element={<FactionListPage />} />

        <Route path="/" element={<ExternalRedirect to="/" />} />
        <Route path="/Domain" element={<ExternalRedirect to="/Domain" />} />
        <Route path="/History" element={<ExternalRedirect to="/History" />} />
        <Route path="/Organizations" element={<ExternalRedirect to="/Organizations" />} />
        <Route path="/Organizations/Take/:id" element={<OrganizationsTakeRedirect />} />
        <Route path='/app/registration' element={<RegistrationPage isLogin={true} />} />
        <Route path='/app/logout' element={<LogoutPage />} />
      </Routes>
    </Layout>
  )
}

export default App;