import Layout from './Layout'
import HomePage from './HomePage'
import MapPage from './MapPage';
import { Route, Routes } from 'react-router-dom';

function App() {

  return (
    <Layout>
      <Routes>
        <Route path='/app/' element={<HomePage />} />
        <Route path='/app/map' element={<MapPage />} />
      </Routes>
    </Layout>
  )
}

export default App;