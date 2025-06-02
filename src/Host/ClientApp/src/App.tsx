import Layout from './Layout'
import HomePage from './HomePage'
import { Route, Routes } from 'react-router-dom';

function App() {

  return (
    <Layout>
      <Routes>
        <Route path='/app/' element={<HomePage />} />
        <Route path='/app/map' />
      </Routes>
    </Layout>
  )
}

export default App;