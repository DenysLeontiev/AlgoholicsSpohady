import { Photo } from "./photo";

export interface Memory {
    id: string;
    title: string;
    description: string;
    memoryUrl: string;
    memoryQrCode: string;
    ownerUserName: string;
    dateCreated: string;
    photos: Photo[];
}