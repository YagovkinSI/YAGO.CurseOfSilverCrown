import React from 'react';
import { useNavigate } from 'react-router-dom';
import { ArrowLeft } from 'lucide-react';
import Text from '../shared/ui/Text';
import Title from '../shared/ui/Title';
import Card from '../shared/ui/Card';
import Page from '../widgets/Page';
import AuthForm from '../features/AuthForm';

const ConvertAccountPage: React.FC = () => {
    const navigate = useNavigate();

    const renderHeader = () => (
        <div className="flex items-center justify-between w-full">
            <button
                onClick={() => navigate('/')}
                className="flex items-center gap-2 text-muted hover:text-light transition-colors"
            >
                <ArrowLeft className="w-4 h-4" />
                <span className="text-sm">Назад</span>
            </button>
        </div>
    );

    const renderTitle = () => (
        <div className="text-center">
            <Title>Сохранение</Title>
            <Text variant="secondary" size="sm" className="mt-1">
                {'Сохраните свой аккаунт'}
            </Text>
        </div>
    );

    const renderForm = () => (
        <AuthForm mode={'convert'}  />
    );

    const renderFooter = () => (
        <Text variant="glass-dim" size="xs" className="mt-2">
            {'Сохраните аккаунт, чтобы не потерять прогресс'}
        </Text>
    );

    const renderContent = () => (
        <div className='h-full overflow-y-auto scrollbar-hide'>
            <div className="flex flex-l items-center justify-center w-full min-h-full py-2">
                <div className="flex items-center justify-center w-full h-full px-4">
                    <Card variant="glow" className="flex flex-col items-center max-w-md w-full">
                        {renderHeader()}
                        {renderTitle()}
                        {renderForm()}
                        {renderFooter()}
                    </Card>
                </div>
            </div>
        </div>
    );

    return (
        <Page backgroundImage='city_in_space' isLoading={false} error={undefined}>
            {renderContent()}
        </Page>
    );
};

export default ConvertAccountPage;