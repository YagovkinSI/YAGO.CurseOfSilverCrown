import Layout from './Layout'
import HomePage from './pages/HomePage'
import { Route, Routes } from 'react-router-dom';
import DevelopingPage from './pages/DevelopingPage';
import RegistrationPage from './pages/RegistrationPage';
import LogoutPage from './pages/LogoutPage';

function App() {
  return (
    <Layout>
      <Routes>
        <Route index element={<HomePage />} />
        <Route path='/' element={<HomePage />} />
        <Route path='/registration' element={<RegistrationPage isLogin={true} />} />
        <Route path='/logout' element={<LogoutPage />} />
        <Route path='/game' element={<DevelopingPage />} />
      </Routes>
    </Layout>
  )
}

export default App;