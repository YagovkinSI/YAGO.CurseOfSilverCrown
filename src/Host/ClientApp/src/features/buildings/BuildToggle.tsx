import React from 'react';
import Surface from '../../shared/ui/Surface';

interface BuildToggleProps {
    isPrivate: boolean;
    onToggle: (value: boolean) => void;
}

const BuildToggle: React.FC<BuildToggleProps> = ({ isPrivate, onToggle }) => (
    <Surface rounded="md" variant="default" className="w-full mb-3">
        <div className="flex items-center gap-2">
            <div className="flex bg-dark border border-bright/15 rounded-lg p-0.5 flex-1">
                <button
                    onClick={() => onToggle(false)}
                    className={`
                        flex-1 py-1.5 px-3 rounded-md text-sm font-medium transition-colors
                        ${isPrivate ? 'text-muted hover:text-light' : 'bg-bright text-dark'}
                    `}
                >
                    Бюджетные
                </button>
                <button
                    onClick={() => onToggle(true)}
                    className={`
                        flex-1 py-1.5 px-3 rounded-md text-sm font-medium transition-colors
                        ${isPrivate ? 'bg-bright text-dark' : 'text-muted hover:text-light'}
                    `}
                >
                    Частные
                </button>
            </div>
        </div>
    </Surface>
);

export default BuildToggle;