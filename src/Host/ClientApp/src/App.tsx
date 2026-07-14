import Layout from './Layout'
import HomePage from './pages/HomePage'
import { Route, Routes } from 'react-router-dom';
import RegistrationPage from './pages/RegistrationPage';
import StatisticsPage from './pages/StatisticsPage';
import ColonyPage from './pages/ColonyPage';
import RatingPage from './pages/RatingPage';
import WikiPage from './pages/WikiPage';
import DeactivateColony from './pages/DeactivateColonyPage';
import ReformsPage from './pages/ReformsPage';
import EventsPage from './pages/EventsPage';
import EventPage from './pages/EventPage';
import UnderDevelopmentPage from './pages/UnderDevelopmentPage';

function App() {
  return (
    <Layout>
      <Routes>
        <Route index element={<HomePage />} />
        <Route path='/' element={<HomePage />} />

        <Route path='/registration' element={<RegistrationPage />} />

        <Route path='/me/colony' element={<ColonyPage />} />
        <Route path='/rating' element={<RatingPage />} />
        <Route path='/wiki/:entityType?/:id?' element={<WikiPage />} />
        <Route path='/more' element={<UnderDevelopmentPage />} />
        
        <Route path='/me/events' element={<EventsPage />} />
        <Route path='/me/construction' element={<UnderDevelopmentPage />} />
        <Route path='/me/reforms' element={<ReformsPage />} />
        <Route path='/me/statistics' element={<StatisticsPage />} />
        <Route path='/me/settings' element={<UnderDevelopmentPage />} />

        <Route path='/me/events/:id?' element={<EventPage />} />

        <Route path='/colony-actions/deactivateColony' element={<DeactivateColony />} />
      </Routes>
    </Layout>
  )
}

export default App;