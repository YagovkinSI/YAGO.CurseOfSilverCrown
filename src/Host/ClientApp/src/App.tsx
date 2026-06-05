import Layout from './Layout'
import HomePage from './pages/HomePage'
import { Route, Routes } from 'react-router-dom';
import RegistrationPage from './pages/RegistrationPage';
import LogoutPage from './pages/LogoutPage';
import StatePage from './pages/StatePage';
import MyColonyPage from './pages/MyColonyPage';
import ColonyRaitingPage from './pages/ColonyRaitingPage';
import WikiPage from './pages/WikiPage';
import DeactivateColony from './pages/DeactivateColony';
import DecreePage from './pages/DecreePage';
import MyQuestListPage from './pages/MyQuestListPage';
import MyQuestPage from './pages/MyQuestPage';

function App() {
  return (
    <Layout>
      <Routes>
        <Route index element={<HomePage />} />
        <Route path='/' element={<HomePage />} />
        <Route path='/registration' element={<RegistrationPage isLogin={true} />} />
        <Route path='/logout' element={<LogoutPage />} />
        <Route path='/me/colony' element={<MyColonyPage />} />
        <Route path='/state' element={<StatePage />} />
        <Route path='/decree' element={<DecreePage />} />
        <Route path='/colonyRaiting' element={<ColonyRaitingPage />} />
        <Route path='/wiki/:entityType?/:id?' element={<WikiPage />} />
        <Route path='/colony-actions/deactivateColony' element={<DeactivateColony />} />
        <Route path='/me/quests' element={<MyQuestListPage />} />
        <Route path='/me/quest/:id?' element={<MyQuestPage />} />
      </Routes>
    </Layout>
  )
}

export default App;