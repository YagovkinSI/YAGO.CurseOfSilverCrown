import Layout from './Layout'
import HomePage from './pages/HomePage'
import { Route, Routes } from 'react-router-dom';
import ExternalRedirect from './pages/ExternalRedirect';
import GamePage from './pages/GamePage';
import RegistrationPage from './pages/RegistrationPage';
import LogoutPage from './pages/LogoutPage';

function App() {
  return (
    <Layout>
      <Routes>
        <Route path='/app' element={<HomePage />} />
        <Route path='/app/registration' element={<RegistrationPage isLogin={true} />} />
        <Route path='/app/logout' element={<LogoutPage />} />
        <Route path='/app/game' element={<GamePage />} />
        
        <Route path="/" element={<ExternalRedirect to="/" />} />
      </Routes>
    </Layout>
  )
}

export default App;