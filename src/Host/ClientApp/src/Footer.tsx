import * as React from 'react';

const Footer: React.FC = () => {
    return (
        <footer className="fixed bottom-0 left-0 right-0 z-[100] bg-dark/90 border-t-2 border-bright text-light py-2 sm:py-3">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <p className="text-muted text-sm text-center">
                    Яговкин С.А., 2025 - 2026
                </p>
            </div>
        </footer>
    );
};

export default Footer;