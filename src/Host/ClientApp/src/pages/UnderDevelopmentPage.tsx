import React from 'react';
import { ArrowLeft, Construction } from 'lucide-react';
import Card from '../shared/Card';
import Divider from '../shared/Divider';
import PageContainer from '../widgets/ContainerPage';
import IconAnimated from '../shared/IconAnimated';
import Title from '../shared/Title';
import Text from '../shared/Text';
import Button from '../shared/Button';
import { useNavigate } from 'react-router-dom';

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
        <>
            <Card variant="glow">
                {renderIcon()}
                <Title>В разработке</Title>
                {renderSubtitle()}
                {renderActions()}
            </Card>
            <Divider />
        </>
    );

    const isLoading = false;
    const error = undefined;
    return (
        <PageContainer backgroundImage='grayСorridor' isLoading={isLoading} error={error}>
            {renderContent()}
        </PageContainer>
    );
};

export default UnderDevelopmentPage;