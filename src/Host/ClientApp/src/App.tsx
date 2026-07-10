import Layout from './Layout'
import HomePage from './pages/HomePage'
import { Route, Routes } from 'react-router-dom';
import RegistrationPage from './pages/RegistrationPage';
import StatePage from './pages/StatePage';
import GamePage from './pages/GamePage';
import ColonyRaitingPage from './pages/ColonyRaitingPage';
import WikiPage from './pages/WikiPage';
import DeactivateColony from './pages/DeactivateColony';
import DecreePage from './pages/DecreePage';
import EventsPage from './pages/EventsPage';
import MyQuestPage from './pages/MyQuestPage';
import UnderDevelopmentPage from './pages/UnderDevelopmentPage';

function App() {
  return (
    <Layout>
      <Routes>
        <Route index element={<HomePage />} />
        <Route path='/' element={<HomePage />} />

        <Route path='/registration' element={<RegistrationPage />} />

        <Route path='/me/colony' element={<GamePage />} />
        <Route path='/rating' element={<ColonyRaitingPage />} />
        <Route path='/wiki/:entityType?/:id?' element={<WikiPage />} />
        <Route path='/more' element={<UnderDevelopmentPage />} />
        
        <Route path='/me/events' element={<EventsPage />} />
        <Route path='/me/construction' element={<UnderDevelopmentPage />} />
        <Route path='/me/reforms' element={<DecreePage />} />
        <Route path='/me/statistics' element={<StatePage />} />
        <Route path='/me/settings' element={<UnderDevelopmentPage />} />

        <Route path='/me/events/:id?' element={<MyQuestPage />} />

        <Route path='/colony-actions/deactivateColony' element={<DeactivateColony />} />
      </Routes>
    </Layout>
  )
}

export default App;