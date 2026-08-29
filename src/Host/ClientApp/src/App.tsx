import Layout from './Layout'
import HomePage from './pages/HomePage'
import { Route, Routes } from 'react-router-dom';
import RegistrationPage from './pages/RegistrationPage';
import StatisticsPage from './pages/StatisticsPage';
import ColonyPage from './pages/ColonyPage';
import RatingPage from './pages/RatingPage';
import WikiPage from './pages/WikiPage';
import ReformsPage from './pages/ReformsPage';
import EventsPage from './pages/EventsPage';
import EventPage from './pages/EventPage';
import UnderDevelopmentPage from './pages/UnderDevelopmentPage';
import ConstructionPage from './pages/ConstructionPage';
import TurnResultPage from './pages/TurnResultPage';
import ConvertAccountPage from './pages/ConvertAccountPage';
import ReformPage from './pages/ReformPage';
import StatisticInfoPage from './pages/StatisticInfoPage';

function App() {
  return (
    <Layout>
      <Routes>
        <Route index element={<HomePage />} />
        <Route path='/' element={<HomePage />} />

        <Route path='/registration' element={<RegistrationPage />} />
        <Route path='/user/convertToPermanent' element={<ConvertAccountPage />} />

        <Route path='/me/colony' element={<ColonyPage />} />
        <Route path='/rating' element={<RatingPage />} />
        <Route path='/wiki/:entityType?/:id?' element={<WikiPage />} />
        <Route path='/more' element={<UnderDevelopmentPage />} />
        
        <Route path='/me/events' element={<EventsPage />} />
        <Route path='/me/construction' element={<ConstructionPage />} />
        <Route path='/me/reforms' element={<ReformsPage />} />
        <Route path='/me/reforms/:code?' element={<ReformPage />} />
        <Route path='/me/statistics/info' element={<StatisticInfoPage />} />
        <Route path='/me/statistics/:id?' element={<StatisticsPage />} />
        <Route path='/me/settings' element={<UnderDevelopmentPage />} />

        <Route path='/me/turnResult' element={<TurnResultPage />} />

        <Route path='/me/events/:id?' element={<EventPage />} />
      </Routes>
    </Layout>
  )
}

export default App;