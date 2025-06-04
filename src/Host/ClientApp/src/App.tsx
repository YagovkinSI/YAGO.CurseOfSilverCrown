import Layout from './Layout'
import HomePage from './HomePage'
import { Route, Routes } from 'react-router-dom';
import ProvincePage from './ProvincePage';

function App() {

  return (
    <Layout>
      <Routes>
        <Route path='/app/' element={<HomePage />} />
        <Route path='/app/map' />
        <Route path='/app/province/:id?' element={<ProvincePage />} />
      </Routes>
    </Layout>
  )
}

export default App;