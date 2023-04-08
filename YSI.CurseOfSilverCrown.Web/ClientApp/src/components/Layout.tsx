import * as React from 'react';
import Nav from './Nav';
import Header from './Header';
import Footer from './Footer';

export interface LayoutProps {
    children?: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
    return (
        <React.Fragment>
            <Header />
            <Nav />
            {children}
            <Footer />
        </React.Fragment>
    );
}

export default Layout;