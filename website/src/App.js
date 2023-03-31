import './App.css';
import { BrowserRouter, Routes, Route, Outlet } from 'react-router-dom';
import HomePage from './pages/HomePage';
import TopAppBar from './components/TopAppBar';
import AboutPage from './pages/AboutPage';
import SimulationPage from './pages/SimulationPage';

function App() {
    return (
        <div className='App'>
            <BrowserRouter>
                <RoutesComponent />
            </BrowserRouter>
        </div>
    );
}

const pages = [
    { path: "/", name: "Home" },
    { path: "/about", name: "About" },
    { path: "/sim", name: "Simulation" }
];

function RoutesComponent() {
    return (
        <Routes>
            <Route path='/' element={<>
                <TopAppBar pages={pages} />
                <Outlet />
            </>}>
                <Route index element={<HomePage />} />
                <Route path='/about' element={<AboutPage />} />
                <Route path='/sim' element={<SimulationPage />} />
            </Route>
        </Routes>
    );
}

export default App;
