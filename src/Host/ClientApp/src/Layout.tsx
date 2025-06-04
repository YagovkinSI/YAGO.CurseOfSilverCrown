import * as React from 'react';
import Header from './Header';
import Footer from './Footer';
import GameMap from './widgets/GameMap';
import { useLocation } from 'react-router-dom';
import SwitchButton from './features/ReactRazorSwitchButton';

export interface LayoutProps {
    children?: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = (props) => {
    const location = useLocation();
    const shouldShowCard = !(location.pathname === '/app/map' || location.pathname === '/app/map/');

    const content = () => {
        return (
            <div className='content-container'>
                <div className='scrollable'>
                    {props.children}
                </div>
            </div>
        )
    }

    return (
        <React.Fragment>
            <Header />
            <main className='base-block main text-dark'>
                <GameMap />
                <SwitchButton />
                {shouldShowCard && content()}
            </main>
            <Footer />
        </React.Fragment>
    );
}

export default Layout;