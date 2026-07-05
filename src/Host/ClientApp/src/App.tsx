import Layout from './Layout'
import Home from './pages/Home'
import { Route, Routes } from 'react-router-dom';
import Game from './pages/Game';
import UnderDevelopment from './pages/UnderDevelopment';
import Registration from './pages/Registration';
import Events from './pages/Events';

function App() {
  return (
    <Layout>
      <Routes>
        <Route index element={<Home />} />
        <Route path='/' element={<Home />} />
        <Route path='/rating' element={<UnderDevelopment />} />
        <Route path='/wiki' element={<UnderDevelopment />} />
        <Route path='/more' element={<UnderDevelopment />} />

        <Route path='/registration' element={<Registration />} />

        <Route path='/me/colony' element={<Game />} />
        <Route path='/me/events' element={<Events />} />
        <Route path='/me/construction' element={<UnderDevelopment />} />
        <Route path='/me/reforms' element={<UnderDevelopment />} />
        <Route path='/me/statistics' element={<UnderDevelopment />} />
        <Route path='/me/settings' element={<UnderDevelopment />} />
      </Routes>
    </Layout>
  )
}

export default App;