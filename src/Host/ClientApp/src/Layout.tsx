import * as React from 'react';
import Header from './Header';
import Footer from './Footer';

export interface LayoutProps {
    children?: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = (props) => {
    return (
        <React.Fragment>
            <Header />
            <main className='base-block main text-dark'>
                <div className='height-full'>
                    <div className='scrollable'>
                        {props.children}
                    </div>
                </div>
            </main>
            <Footer />
        </React.Fragment>
    );
}

export default Layout;