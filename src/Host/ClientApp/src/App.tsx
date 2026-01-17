import Layout from './Layout'
import HomePage from './pages/HomePage'
import { Route, Routes } from 'react-router-dom';
import RegistrationPage from './pages/RegistrationPage';
import LogoutPage from './pages/LogoutPage';
import StatePage from './pages/StatePage';
import CreateClolonyPage from './pages/CreateClolonyPage';
import MyColonyPage from './pages/MyColonyPage';
import UnitPage from './pages/UnitPage';
import ColonyRaitingPage from './pages/ColonyRaitingPage';
import RunCycle from './pages/RunCyclePage';
import WikiPage from './pages/WikiPage';

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
        <Route path='/unit' element={<UnitPage />} />
        <Route path='/colonyRaiting' element={<ColonyRaitingPage />} />
        <Route path='/colony-actions/runCycle' element={<RunCycle />} />
        <Route path='/wiki/:entityType/:id' element={<WikiPage />} />
      </Routes>
    </Layout>
  )
}

export default App;