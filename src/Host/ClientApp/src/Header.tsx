import * as React from 'react';
import NavBar from './widgets/NavBar';

const Header: React.FC = () => {
    return (
        <header className="fixed top-0 left-0 right-0 z-[100] bg-dark/90 border-b-2 border-bright shadow-[0_5px_5px_rgba(0,0,0,0.5)] text-light">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <NavBar />
            </div>
        </header>
    );
};

export default Header;