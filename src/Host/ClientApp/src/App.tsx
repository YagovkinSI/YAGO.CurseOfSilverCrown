import Layout from './Layout'
import HomePage from './pages/HomePage'
import { Route, Routes } from 'react-router-dom';
import RegistrationPage from './pages/RegistrationPage';
import LogoutPage from './pages/LogoutPage';
import StatePage from './pages/StatePage';
import ShipPage from './pages/ShipPage';
import CreateClolonyPage from './pages/CreateClolonyPage';
import MyColony from './pages/MyColony';

function App() {
  return (
    <Layout>
      <Routes>
        <Route index element={<HomePage />} />
        <Route path='/' element={<HomePage />} />
        <Route path='/registration' element={<RegistrationPage isLogin={true} />} />
        <Route path='/logout' element={<LogoutPage />} />
        <Route path='/createColony' element={<CreateClolonyPage />} />
        <Route path='/me/colony' element={<MyColony />} />
        <Route path='/state' element={<StatePage />} />
        <Route path='/ship' element={<ShipPage />} />
      </Routes>
    </Layout>
  )
}

export default App;