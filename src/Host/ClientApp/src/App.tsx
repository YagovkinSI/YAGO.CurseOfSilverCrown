import Layout from './Layout'
import HomePage from './pages/HomePage'
import { Route, Routes } from 'react-router-dom';
import RegistrationPage from './pages/RegistrationPage';
import LogoutPage from './pages/LogoutPage';
import PrologPage from './pages/PrologPage';
import StatePage from './pages/StatePage';

function App() {
  return (
    <Layout>
      <Routes>
        <Route index element={<HomePage />} />
        <Route path='/' element={<HomePage />} />
        <Route path='/registration' element={<RegistrationPage isLogin={true} />} />
        <Route path='/logout' element={<LogoutPage />} />
        <Route path='/prolog' element={<PrologPage />} />
        <Route path='/state' element={<StatePage />} />
      </Routes>
    </Layout>
  )
}

export default App;