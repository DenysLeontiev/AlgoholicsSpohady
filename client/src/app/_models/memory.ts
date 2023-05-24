import { Message } from "./message";
import { Photo } from "./photo";

export interface Memory {
    id: string;
    title: string;
    description: string;
    memoryUrl: string;
    memoryQrCode: string;
    ownerId: string;
    dateCreated: string;
    photos: Photo[];
    messages: Message[];
}