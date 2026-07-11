import React from 'react';
import { ArrowLeft, Construction } from 'lucide-react';
import Card from '../shared/Card';
import Divider from '../shared/Divider';
import IconAnimated from '../shared/IconAnimated';
import Title from '../shared/Title';
import Text from '../shared/Text';
import Button from '../shared/Button';
import { useNavigate } from 'react-router-dom';
import Page from '../widgets/Page';

const UnderDevelopmentPage: React.FC = () => {
    const navigate = useNavigate();

    const renderIcon = () => (
        <IconAnimated
            icon={Construction}
            color="bright"
            size="md"
            pingOpacity={0.2}
        />
    );

    const renderSubtitle = () => (
        <Text variant="secondary">
            Эта страница ещё создаётся.
            <br />
            Скоро здесь появится что-то интересное.
        </Text>
    );

    const renderActions = () => (
        <div className="flex flex-col items-center gap-3 w-full max-w-xs">
            <Button
                variant="secondary"
                size="sm"
                icon={ArrowLeft}
                onClick={() => navigate(-1)}
            >
                Назад
            </Button>
        </div>
    );

    const renderContent = () => (
        <div className="flex flex-l items-center justify-center w-full min-h-full py-2">
            <Card variant="glow">
                {renderIcon()}
                <Title>В разработке</Title>
                {renderSubtitle()}
                {renderActions()}
            </Card>
            <Divider />
        </div>
    );

    const isLoading = false;
    const error = undefined;
    return (
        <Page backgroundImage='grayСorridor' isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default UnderDevelopmentPage;