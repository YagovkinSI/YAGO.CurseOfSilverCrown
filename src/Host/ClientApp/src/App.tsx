import Layout from './Layout'
import HomePage from './pages/HomePage'
import { Route, Routes } from 'react-router-dom';
import RegistrationPage from './pages/RegistrationPage';
import LogoutPage from './pages/LogoutPage';
import StatePage from './pages/StatePage';
import ShipPage from './pages/ShipPage';
import CreateClolonyPage from './pages/CreateClolonyPage';
import MyColonyPage from './pages/MyColonyPage';
import BuildingPage from './pages/BuildingPage';
import ColonyRaitingPage from './pages/ColonyRaitingPage';

function App() {
  return (
    <Layout>
      <Routes>
        <Route index element={<HomePage />} />
        <Route path='/' element={<HomePage />} />
        <Route path='/registration' element={<RegistrationPage isLogin={true} />} />
        <Route path='/logout' element={<LogoutPage />} />
        <Route path='/createColony' element={<CreateClolonyPage />} />
        <Route path='/me/colony' element={<MyColonyPage />} />
        <Route path='/state' element={<StatePage />} />
        <Route path='/ship' element={<ShipPage />} />
        <Route path='/building' element={<BuildingPage />} />
        <Route path='/colonyRaiting' element={<ColonyRaitingPage />} />
      </Routes>
    </Layout>
  )
}

export default App;