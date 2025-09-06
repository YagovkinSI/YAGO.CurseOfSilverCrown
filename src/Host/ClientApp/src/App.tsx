import Layout from './Layout'
import HomePage from './pages/HomePage'
import { Route, Routes } from 'react-router-dom';
import GamePage from './pages/GamePage';
import RegistrationPage from './pages/RegistrationPage';
import LogoutPage from './pages/LogoutPage';
import StoryListPage from './pages/StoryListPage';
import StoryPage from './pages/StoryPage';
import CreateCharacterPage from './pages/CreateCharacterPage';

function App() {
  return (
    <Layout>
      <Routes>
        <Route index element={<HomePage />} />
        <Route path='/' element={<HomePage />} />
        <Route path='/registration' element={<RegistrationPage isLogin={true} />} />
        <Route path='/logout' element={<LogoutPage />} />
        <Route path='/createCharacter' element={<CreateCharacterPage />} />
        <Route path='/game' element={<GamePage />} />
        <Route path='/storyList' element={<StoryListPage />} />
        <Route path='/story' element={<StoryPage />} />
      </Routes>
    </Layout>
  )
}

export default App;