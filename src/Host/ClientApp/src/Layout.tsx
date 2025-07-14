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
                {content()}
            </main>
            <Footer />
        </React.Fragment>
    );
}

export default Layout;