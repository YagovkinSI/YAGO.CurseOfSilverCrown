import * as React from 'react';
import Header from './Header';
import Footer from './Footer';

export interface LayoutProps {
    children?: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = (props) => {

    const content = () => {
        return (
            <div className='content-container'>
                {props.children}
            </div>
        )
    }

    return (
        <div>
            <Header />
            <main className='base-block main text-dark'>
                {content()}
            </main>
            <Footer />
        </div>
    );
}

export default Layout;